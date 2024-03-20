// Copyright (c) Microsoft Corporation.
//  Licensed under the MIT License.

using RulesEngine.ExpressionBuilders;
using RulesEngine.Models;
using System.Threading.Tasks;

namespace RulesEngine.Actions
{
    public class OperationExpressionAction : ActionBase
    {
        private readonly RuleExpressionParser _ruleExpressionParser;

        public OperationExpressionAction(RuleExpressionParser ruleExpressionParser)
        {
            _ruleExpressionParser = ruleExpressionParser;
        }

        public override ValueTask<object> Run(ActionContext context, RuleParameter[] ruleParameters)
        {
            var expression = context.GetContext<string>("expression");
            _ruleExpressionParser.Evaluate(expression, ruleParameters);
            return new ValueTask<object>(true);
        }
    }
}
