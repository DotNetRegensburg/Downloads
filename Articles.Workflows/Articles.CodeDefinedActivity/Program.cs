using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Statements;

namespace Articles.CodeDefinedActivity
{
    public class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        public static void Main()
        {
            //Define sample sequence
            Variable<string> inputFromCmd = new Variable<string>();
            Sequence mySequence = new Sequence()
            {
                Variables = 
                {
                    inputFromCmd
                },
                Activities =
                {
                    new WriteLine()
                    {
                        Text = "Workflow is starting.."
                    },
                    new Delay()
                    {
                         Duration = TimeSpan.FromSeconds(2.0)
                    },
                    new WriteLine()
                    {
                        Text = "Please type some text:"
                    },
                    new InvokeMethod<string>()
                    {
                        MethodName = "ReadLine",
                        TargetType = typeof(Console),
                        Result = new LambdaReference<string>((context) => inputFromCmd.Get(context)),
                        RunAsynchronously = true
                    },
                    new WriteLine()
                    {
                        Text = new LambdaValue<string>((context) => "Input from user: " + inputFromCmd.Get(context))
                    },
                    new WriteLine()
                    {
                        Text = "Workflow finished"
                    }
                }
            };

            //Execute sequence
            for (int loop = 0; loop < 100; loop++)
            {
                WorkflowInvoker.Invoke(mySequence);
            }
        }
    }
}
