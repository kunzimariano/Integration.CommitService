using CommitService.Contract;
using Infrastructure.Composition;
using ServiceStack.Common.Utils;
using ServiceStack.Messaging;
using ServiceStack.Redis;
using ServiceStack.Redis.Messaging;
using ServiceStack.WebHost.Endpoints;

[assembly: WebActivator.PreApplicationStartMethod(typeof(CommitService.App_Start.AppHost), "Start")]

namespace CommitService.App_Start
{
    public class AppHost
        : AppHostBase
    {
        public AppHost()
            : base("Commit Service", typeof(CommitService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            PathProvider.BinaryPath = "~".MapAbsolutePath();

            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

            Routes
                .Add<CommitAttempt>("/commit")
                .Add<CommitMessages>("/commits")

            //    //.Add<CommitMessage>("/commitMessage")
              ;

            RequestBinders.Add(typeof(CommitAttempt), request => new CommitAttempt() { Raw = request.GetRawBody() });

            var redisFactory = new PooledRedisClientManager("localhost:6379");
            //var mqHost = new RedisMqHost(redisFactory);
            var mqHost = new RedisMqServer(redisFactory);

            container.Register<IMessageService>(mqHost);
            container.Register(mqHost.MessageFactory);

            mqHost.RegisterHandler<CommitAttempt>(ServiceController.ExecuteMessage);
            //mqHost.RegisterHandler<CommitMessage>(ServiceController.ExecuteMessage);

            mqHost.Start();
        }

        public static void Start()
        {
            new AppHost().Init();
        }
    }
}
