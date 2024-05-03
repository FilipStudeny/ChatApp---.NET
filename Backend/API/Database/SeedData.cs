﻿using API.Services;
using API.Services.Helpers;
using MongoDB.Bson;
using Shared.Enums;
using Shared.Models;

namespace API.Database;

public class SeedData : PasswordService
{
    private readonly ObjectId _user1Id = ObjectId.GenerateNewId();
    private readonly ObjectId _user2Id = ObjectId.GenerateNewId();
    private readonly ObjectId _user3Id = ObjectId.GenerateNewId();

    public IEnumerable<User> SeedUsers()
    {
        var user1 = new User
        {
            Id = _user1Id,
            Email = "toni.peterson@example.com",
            Username = "toni123",
            FirstName = "Toni",
            LastName = "Peterson",
            RegisterDate = DateTime.Now,
            LastActivity = DateTime.Now,
            ProfilePicture =
                "data:image/jpeg;base64,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",
            Password = SeedPassword("colonel"),
            Gender = Gender.Female,
            
        };
        var user2 = new User
        {
            Id = _user2Id,
            Email = "henry.graham@example.com",
            FirstName = "Henry",
            LastName = "Graham",
            Username = "henryG",
            RegisterDate = DateTime.Now,
            LastActivity = DateTime.Now,
            ProfilePicture =
                "data:image/jpeg;base64,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",
            Password = SeedPassword("colonel"),
            Gender = Gender.Male,
        };

        user1.Friends =
        [
            new Friend
            {
                Id = user2.Id,
                Username = user2.Username
            }
        ];
        user2.Friends =
        [
            new Friend
            {
                Id = user1.Id,
                Username = user1.Username
            }
        ];

        return [user1, user2];
    }

    private PasswordInfo SeedPassword(string password)
    {
        ((IPasswordService)this).CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        return new PasswordInfo
        {
            Salt = passwordSalt,
            Hash = passwordHash
        };
    }

}