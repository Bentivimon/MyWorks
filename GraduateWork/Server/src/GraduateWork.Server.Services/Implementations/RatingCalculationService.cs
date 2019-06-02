using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraduateWork.Server.Data;
using GraduateWork.Server.Models.Enums;
using GraduateWork.Server.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GraduateWork.Server.Services.Implementations
{
    /// <inheritdoc/>
    public class RatingCalculationService : IRatingCalculationService
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// Basic constructor.
        /// </summary>
        public RatingCalculationService(DatabaseContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task CalculateAsync(CancellationToken cancellationToken)
        {
            if (await _context.Statements.AllAsync(x=> x.Status != StatementStatus.Holding, cancellationToken).ConfigureAwait(false))
                return;

            var specialties = await _context.Specialties.ToListAsync(cancellationToken).ConfigureAwait(false);

            foreach (var specialityEntity in specialties)
            {
                var statements = await _context.Statements
                    .Where(y => y.SpecialityId == specialityEntity.Id && y.Priority == 1).OrderByDescending(x => x.TotalScore)
                    .ToListAsync(cancellationToken).ConfigureAwait(false);

                if (!statements.Any())
                    continue;

                for (var i = 0; i < statements.Count; i++)
                {
                    if (i + 1 < specialityEntity.CountOfPlaces)
                    {
                        statements[i].Status = StatementStatus.Accepted;
                        await _context.Statements
                            .Where(x => x.EntrantId == statements[i].EntrantId && x.Id != statements[i].Id)
                            .ForEachAsync(
                                x => x.Status = StatementStatus.Rejected, cancellationToken).ConfigureAwait(false);
                    }
                }
                
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                await _context.Statements
                    .Where(x => x.SpecialityId == specialityEntity.Id && x.Priority == 1 &&
                                x.Status != StatementStatus.Accepted)
                    .ForEachAsync(x => x.Status = StatementStatus.Rejected, cancellationToken).ConfigureAwait(false);

                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            var isSomethingChanged = true;
            while (isSomethingChanged)
            {
                isSomethingChanged = false;

                for (var i = 2; i < 16; i++)
                {
                    var entrantIds = await _context.Entrants.Include(x => x.Statements)
                        .Where(x => x.Statements.All(y => y.Status != StatementStatus.Accepted)).Select(x => x.Id).ToListAsync(cancellationToken)
                        .ConfigureAwait(false);

                    if (!entrantIds.Any())
                        continue;

                    var statements = await _context.Statements
                        .Where(x => entrantIds.Contains(x.EntrantId) && x.Status == StatementStatus.Holding && x.Priority == i).ToListAsync(cancellationToken)
                        .ConfigureAwait(false);

                    foreach (var statementEntity in statements)
                    {
                        var speciality = specialties.First(x => x.Id == statementEntity.SpecialityId);

                        var currentSpecialityStatements = await _context.Statements.AsNoTracking()
                            .Where(x => x.Status == StatementStatus.Accepted && x.SpecialityId == speciality.Id)
                            .OrderByDescending(x => x.TotalScore)
                            .ToListAsync(cancellationToken).ConfigureAwait(false);

                        if (speciality.CountOfPlaces > currentSpecialityStatements.Count)
                        {
                            statementEntity.Status = StatementStatus.Accepted;
                            await _context.Statements
                                .Where(x => x.EntrantId == statementEntity.EntrantId && x.Id != statementEntity.Id)
                                .ForEachAsync(
                                    x => x.Status = StatementStatus.Rejected, cancellationToken).ConfigureAwait(false);
                            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                            isSomethingChanged = true;
                            continue;
                        }

                        var lastTotalScore = currentSpecialityStatements.LastOrDefault()?.TotalScore ?? 0d;

                        if (lastTotalScore >= statementEntity.TotalScore)
                        {
                            statementEntity.Status = StatementStatus.Rejected;
                            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                            isSomethingChanged = true;
                            continue;
                        }

                        statementEntity.Status = StatementStatus.Accepted;
                        await _context.Statements
                            .Where(x => x.EntrantId == statementEntity.EntrantId && x.Id != statementEntity.Id)
                            .ForEachAsync(
                                x => x.Status = StatementStatus.Rejected, cancellationToken).ConfigureAwait(false);
                        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                        currentSpecialityStatements.Add(statementEntity);
                        currentSpecialityStatements = currentSpecialityStatements.OrderByDescending(x => x.TotalScore).ToList();

                        var lastStatements = currentSpecialityStatements.Last();

                        currentSpecialityStatements.Remove(lastStatements);

                        var lastStatementsEntity = await _context.Statements
                            .FirstAsync(x => x.Id == lastStatements.Id, cancellationToken)
                            .ConfigureAwait(false);

                        lastStatementsEntity.Status = StatementStatus.Rejected;
                        await _context.Statements
                            .Where(x => x.EntrantId == lastStatementsEntity.EntrantId && x.Id != lastStatementsEntity.Id && x.Priority > lastStatementsEntity.Priority)
                            .ForEachAsync(
                                x => x.Status = StatementStatus.Holding, cancellationToken).ConfigureAwait(false);
                        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                        isSomethingChanged = true;
                    }
                }
            }
        }
    }
}
