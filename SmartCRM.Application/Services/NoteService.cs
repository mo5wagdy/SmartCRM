using AutoMapper;
using SmartCRM.Application.Dtos.Note_Dtos;
using SmartCRM.Application.Exceptions;
using SmartCRM.Application.Interfaces.Repositories;
using SmartCRM.Application.Interfaces.Services;
using SmartCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public NoteService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<NoteDto> CreateAsync(CreateNoteDto dto)
        {
            var note = _mapper.Map<Note>(dto);
            note.CreatedAt = DateTime.UtcNow;
            note.IsActive = true;
            note.IsDeleted = false;
            await _uow.Notes.AddAsync(note);
            await _uow.SaveAsync();
            //TODO: Trigger domain event/notification if needed
            return _mapper.Map<NoteDto>(note);
        }

        public async Task<IEnumerable<NoteDto>> GetAllAsync(int Page, int PageSize, string? Q, string? RelatedTo, int? RelatedId, int? CustomerId, int? DealId, int? UserId, DateTime? From, DateTime? To)
        {
            var query = _uow.Notes.QueryNoTracking().Where(n => !n.IsDeleted);

            if (!string.IsNullOrEmpty(Q))
                query = query.Where(n => n.Content.Contains(Q));
            if (!string.IsNullOrEmpty(RelatedTo))
                query = query.Where(n => n.RelatedTo == RelatedTo);
            if (RelatedId.HasValue)
                query = query.Where(n => n.RelatedId == RelatedId);
            if (CustomerId.HasValue)
                query = query.Where(n => n.CustomerId == CustomerId);
            if (DealId.HasValue)
                query = query.Where(n => n.DealId == DealId);
            if (UserId.HasValue)
                query = query.Where(n => n.UserId == UserId);
            if (From.HasValue)
                query = query.Where(n => n.CreatedAt >= From.Value);
            if (To.HasValue)
                query = query.Where(n => n.CreatedAt <= To.Value);

            var PagedQuery = query
                .OrderByDescending(n => n.CreatedAt)
                .Skip((Page - 1) * PageSize)
                .Take(PageSize);

            var notes = await _uow.Notes.ToListAsync(PagedQuery);

            return _mapper.Map<IEnumerable<NoteDto>>(notes);

        }

        public async Task<NoteDto?> GetByIdAsync(int Id)
        {
            var note =  await _uow.Notes.GetByIdAsync(Id); 
            if (note == null || note.IsDeleted) return null;
            return _mapper.Map<NoteDto>(note);
        }

        public async Task LogCustomerActivityAsync(int CustomerId, string Content, int? UserId)
        {
            var note = new Note
            {
                Content = Content,
                RelatedTo = "Customer",
                RelatedId = CustomerId,
                CustomerId = CustomerId,
                UserId = UserId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };
            await _uow.Notes.AddAsync(note);
            await _uow.SaveAsync();
        }

        public async Task LogDealStageChangeAsync(int DealId, string FromStage, string ToStage, int? UserId)
        {
            var note = new Note
            {
                Content = $"Deal stage changed from {FromStage} to {ToStage}.",
                RelatedTo = "Deal",
                RelatedId = DealId,
                DealId = DealId,
                UserId = UserId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };
            await _uow.Notes.AddAsync(note);
            await _uow.SaveAsync();
        }

        public async Task SoftDeleteAsync(int Id)
        {
            var note = await _uow.Notes.GetByIdAsync(Id);
            if (note == null || note.IsDeleted) throw new NotFoundException($"Note {Id} not found");
            note.IsDeleted = true;
            await _uow.SaveAsync();
            // TODO: Audit logging if needed
        }

        public async Task UpdateAsync(int Id, UpdateNoteDto dto)
        {
            var note = await _uow.Notes.GetByIdAsync(Id);
            if (note == null || note.IsDeleted) throw new NotFoundException($"Note {Id} not found");

            note.Content = dto.Content;
            note.RelatedTo = dto.RelatedTo;
            note.RelatedId = dto.RelatedId;
            note.CustomerId = dto.CustomerId;
            note.DealId = dto.DealId;
            note.DealId = dto.DealId;
            note.UserId = dto.UserId;
            note.UpdatedAt = DateTime.UtcNow;

            await _uow.Notes.UpdateAsync(note);
            await _uow.SaveAsync();
            //TODO: Audit logging if needed

        }
    }
}
