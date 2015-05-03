﻿using Autofac;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Tests.TestRig
{
    public class ProjectGameScriptEngine : BaseScriptEngine
    {
        public ProjectGameScriptEngine(IGameEngine gameEngine) 
            : base(gameEngine)
        {
        }

        protected override void OnInitializeEngine()
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