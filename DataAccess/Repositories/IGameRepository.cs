﻿using DataAccess.Models;

namespace DataAccess.Repositories;

public interface IGameRepository : IRepository<Game>{ 
    Task<Game?> GetByAppId(string appId);
}