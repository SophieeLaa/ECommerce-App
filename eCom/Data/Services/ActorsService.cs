﻿using eCom.Models;
using Microsoft.EntityFrameworkCore;

namespace eCom.Data.Services
{
    public class ActorsService : IActorsService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ActorsService> _logger;
        public ActorsService(AppDbContext context, ILogger<ActorsService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(Actor actor)
        {
            try
            {
                await _context.Actors.AddAsync(actor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding actor to the database.");
                throw; // Rethrow the exception after logging
            }
        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.Actors.FirstOrDefaultAsync(n => n.Id == id);
            _context.Actors.Remove(result);
            await _context.SaveChangesAsync();
        }

      
        public async Task<Actor> UpdateAsync(int id, Actor newActor)
        {
            _context.Update(newActor);
            await _context.SaveChangesAsync();
            return newActor;
        }
    }
}
