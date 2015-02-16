using System;
using System.Collections.Generic;
using Autofac;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Tests.TestRig
{
    public class  GameScriptEngine : BaseScriptEngine
    {
        public GameScriptEngine(Config config) : base(config)
        {
        }

        public override void RegisterComponents(ContainerBuilder builder)
        {
            builder.Register(context => this)
                .As<IScriptEngine>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TestScriptParmaterProvider>()
                .As<IScriptParamaterProvider>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TestLogger>()
                   .As<ILogger>()
                   .InstancePerLifetimeScope();
        }
    }
}
