﻿namespace CommunityApp.Data.Repositories.Interfaces
{
    public interface ICommunityManagerRepository
    {
        Task<bool> AddManagerToCommunityAsync(string managerId, int communityId);
        Task<bool> RemoveManagerFromCommunityAsync(string managerId, int communityId);
    }
}