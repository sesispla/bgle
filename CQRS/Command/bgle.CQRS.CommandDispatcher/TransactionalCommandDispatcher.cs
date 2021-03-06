﻿using System;
using System.Threading.Tasks;

using bgle.ComponentModel.DataAnnotations;
using bgle.Contracts.UnitOfWork;
using bgle.CQRS.CommandDispatcher.Factory;
using bgle.CQRS.CommandHandler;

namespace bgle.CQRS.CommandDispatcher
{
    public class TransactionalCommandDispatcher : CommandDispatcher
    {
        private readonly IScopedCommandHandlerFactory factory;

        private IUnitOfWork unitOfWork;

        public TransactionalCommandDispatcher(IScopedCommandHandlerFactory factory)
            : base(factory)
        {
            this.factory = factory;
        }

        public override ValidationResultCollection Dispatch<TCommand>(TCommand command)
        {
            using (var scope = this.factory.BeginScope())
            {
                this.unitOfWork = scope.UnitOfWork;
                return base.Dispatch(command);
            }
        }

        public override Task<ValidationResultCollection> SubmitAsync<TCommand>(TCommand command)
        {
            using (var scope = this.factory.BeginScope())
            {
                this.unitOfWork = scope.UnitOfWork;
                return base.SubmitAsync(command);
            }
        }

        protected override void Send<TCommand>(TCommand command, ICommandHandler<TCommand> commandHandler)
        {
            try
            {
                this.unitOfWork.BeginTransaction();
                base.Send(command, commandHandler);
                this.unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                this.unitOfWork.RollBackTransaction();
                throw;
            }
        }
    }
}