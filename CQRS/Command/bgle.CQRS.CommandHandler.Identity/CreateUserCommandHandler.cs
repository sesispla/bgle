﻿using bgle.Contracts.DateTimeHandling;
using bgle.Contracts.Repository;
using bgle.Core.Identity;
using bgle.CQRS.Command.Identity;
using bgle.CQRS.Event;
using bgle.CQRS.EventStore;

namespace bgle.CQRS.CommandHandler.Identity
{
    public class CreateUserCommandHandler : UpsertCommandHandler<CreateUserCommand, IdentityUser, string, EmptyEvent>
    {
        public CreateUserCommandHandler(IEventStore eventStore, IRepository repository, IDateTimeProvider dateTimeProvider)
            : base(eventStore, repository, dateTimeProvider)
        {
        }

        protected override void FillEntity(CreateUserCommand command, IdentityUser entity)
        {
            entity.UserName = command.UserName;
            entity.PasswordHash = command.PasswordHash;
            entity.Email = command.Email;
            entity.SecurityStamp = command.SecurityStamp;
            entity.EmailConfirmed = command.EmailConfirmed;
            entity.LockoutEndDate = command.LockoutEndDate;
            entity.AccessFailedCount = command.AccessFailedCount;
            entity.LockoutEnabled = command.LockoutEnabled;
        }
    }
}