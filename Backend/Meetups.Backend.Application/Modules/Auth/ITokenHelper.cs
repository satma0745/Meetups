namespace Meetups.Backend.Application.Modules.Auth;

using System;
using System.Collections.Generic;
using Meetups.Backend.Domain.Entities.User;

public interface ITokenHelper
{
    bool TryParseToken(string token, out IDictionary<string, string> payload);

    ITokenPair IssueTokenPair(User user, Guid refreshTokenId);
}