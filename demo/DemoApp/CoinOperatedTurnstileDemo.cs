// Copyright (c) Microsoft Corporation.
//  Licensed under the MIT License.

using Newtonsoft.Json;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static RulesEngine.Extensions.ListofRuleResultTreeExtension;

namespace DemoApp
{
    public class CoinOperatedTurnstileDemo
    {
        public class Event
        {
            public Event(string code)
            {
                Code = code;
            }
            public Event(string code, string name)
            {
                Code = code;
                Name = name;
            }
            public string Code { get; set; }
            public string Name { get; set; }

            public static Event Of(string eventCode)
            {
                return new Event(eventCode);
            }
        }
        public class CoinOperatedTurnstile {
            private static readonly RulesEngine.RulesEngine stateMachine = InitStateMachine();
            public string State { get; private set; } = "Locked";
            static private RulesEngine.RulesEngine InitStateMachine()
            {
                var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "CoinOperatedTurnstile.json", SearchOption.AllDirectories);
                if (files == null || files.Length == 0)
                    throw new FileNotFoundException("State machine schema not found.");

                var fileData = File.ReadAllText(files[0]);
                var workflows = JsonConvert.DeserializeObject<List<Workflow>>(fileData);

                return new RulesEngine.RulesEngine(workflows.ToArray(), null);
            }
            public void ChangeState(string state)
            {
                this.State = state;
            }
            public void TriggerEvent(Event anEvent)
            {
                string stateTransition = "";
                var rp1 = new RuleParameter("cot", this);
                var rp2 = new RuleParameter("event", anEvent);
                Task<List<RuleResultTree>> task = Task.Run(async () => await stateMachine.ExecuteAllRulesAsync("COTStateMachine", rp1, rp2));
                var resultList = task.Result;
                resultList.OnSuccess((eventName) => {
                    stateTransition = $"Coin Operated Turnstile State is {State}.";
                });

                resultList.OnFail(() => {
                    stateTransition = "Coin Operated Turnstile doesn't change State";
                });

                Console.WriteLine(stateTransition);

            }
        }
        public void Run()
        {
            Console.WriteLine($"Running {nameof(CoinOperatedTurnstileDemo)}....");

            var cot = new CoinOperatedTurnstile();

            Console.WriteLine($"Coin Operated Turnstile State is {cot.State}.");
            cot.TriggerEvent(Event.Of("GatePushed"));
            cot.TriggerEvent(Event.Of("DummyEvent"));
            cot.TriggerEvent(Event.Of("CoinInserted"));
            cot.TriggerEvent(Event.Of("CoinInserted"));
            cot.TriggerEvent(Event.Of("GatePushed"));
        }
    }
}
