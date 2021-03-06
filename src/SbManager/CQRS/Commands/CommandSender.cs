﻿using System;
using Autofac;

namespace SbManager.CQRS.Commands
{
    public class CommandSender : ICommandSender
    {
        private readonly ILifetimeScope _lifetimeScope;

        public CommandSender(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public void Send<TCommand>(TCommand command)
            where TCommand : class, ICommand
        {
            if (command == null) throw new NullReferenceException("command");

            var handler = _lifetimeScope.Resolve<ICommandHandler<TCommand>>();

            if (handler == null) throw new InvalidOperationException("Could not resolve command handler for command type " + command.GetType().FullName);

            handler.Execute(command);
        }

        public TResult SendWithResult<TCommand, TResult>(TCommand command)
            where TCommand : class,  ICommand
        {
            if (command == null) throw new NullReferenceException("command");

            var handler = _lifetimeScope.Resolve<ICommandHandlerWithResult<TCommand, TResult>>();

            if (handler == null) throw new InvalidOperationException("Could not resolve command handler for command type " + command.GetType().FullName + " and result type " + typeof(TResult).FullName);

            return handler.ExecuteWithResult(command);
        }
    }
}
