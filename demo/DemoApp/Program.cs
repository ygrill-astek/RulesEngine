// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace DemoApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new CoinOperatedTurnstileDemo().Run();
            new BasicDemo().Run();
            new JSONDemo().Run();
            new NestedInputDemo().Run();
            new EFDemo().Run();
        }
    }
}
