using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GraduateWork.Server.Data;
using GraduateWork.Server.Data.Entities;
using GraduateWork.Server.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraduateWork.Server.Services
{
    /// <summary>
    /// Database seeder.
    /// </summary>
    public class DatabaseSeeder
    {
        private DatabaseContext _context;
        private ICryptoProvider _cryptoProvider;
        
        /// <summary>
        /// Method for seed default data to database.
        /// </summary>
        public async Task SeedAsync(IServiceProvider services)
        {
            _context = services.GetRequiredService<DatabaseContext>();
            _cryptoProvider = services.GetRequiredService<ICryptoProvider>();

            await SeedUsersAsync().ConfigureAwait(false);
            await SeedUniversitiesAsync().ConfigureAwait(false);
            await SeedEntrantsAsync().ConfigureAwait(false);
        }

        private async Task CalculateRatings()
        {
            var specialties = await _context.Specialties.ToListAsync().ConfigureAwait(false);

            foreach (var specialityEntity in specialties)
            {
                var statement = await _context.Statements
                    .Where(y => y.SpecialityId == specialityEntity.Id && y.Priority == 1).OrderBy(x=> x.TotalScore)
                    .ToListAsync().ConfigureAwait(false);

                if(!statement.Any())
                    continue;

                for (var i = 0; i < statement.Count; i++)
                {
                    if(i+1 < specialityEntity.CountOfPlaces)
                        statement[i].IsAccepted = true;
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);
                
            }

            var isSomethingChanged = true;
            while (isSomethingChanged)
            {
                isSomethingChanged = false;
                
                for (var i = 2; i < 16; i++)
                {
                    var entrantIds = await _context.Entrants.Include(x => x.Statements)
                        .Where(x => x.Statements.All(y => !y.IsAccepted)).Select(x => x.Id).ToListAsync()
                        .ConfigureAwait(false);

                    var statements = await _context.Statements
                        .Where(x => entrantIds.Contains(x.EntrantId) && x.Priority == i).ToListAsync()
                        .ConfigureAwait(false);

                    foreach (var statementEntity in statements)
                    {
                        var speciality = await _context.Specialties.AsNoTracking()
                            .FirstAsync(x => x.Id == statementEntity.SpecialityId).ConfigureAwait(false);

                        var currentSpecialityStatements = await _context.Statements.AsNoTracking()
                            .Where(x => x.IsAccepted && x.SpecialityId == speciality.Id).OrderBy(x => x.TotalScore)
                            .ToListAsync().ConfigureAwait(false);

                        var lastTotalScore = currentSpecialityStatements.LastOrDefault()?.TotalScore ?? 0d;

                        if (lastTotalScore > statementEntity.TotalScore)
                        {
                            if (speciality.CountOfPlaces == currentSpecialityStatements.Count)
                                continue;

                            statementEntity.IsAccepted = true;
                            await _context.SaveChangesAsync().ConfigureAwait(false);
                            isSomethingChanged = true;
                            continue;
                        }

                        if (lastTotalScore <= statementEntity.TotalScore &&
                            speciality.CountOfPlaces > currentSpecialityStatements.Count)
                        {
                            statementEntity.IsAccepted = true;
                            await _context.SaveChangesAsync().ConfigureAwait(false);
                            isSomethingChanged = true;
                            continue;
                        }

                        if (Math.Abs(lastTotalScore - statementEntity.TotalScore) < 1e-6)
                            continue;

                        statementEntity.IsAccepted = true;
                        await _context.SaveChangesAsync().ConfigureAwait(false);

                        currentSpecialityStatements.Add(statementEntity);
                        currentSpecialityStatements = currentSpecialityStatements.OrderBy(x => x.TotalScore).ToList();

                        var lastStatements = currentSpecialityStatements.LastOrDefault();

                        if (lastStatements != null)
                        {
                            currentSpecialityStatements.Remove(lastStatements);

                            var lastStatementsEntity = await _context.Statements
                                .FirstAsync(x => x.Id == lastStatements.Id)
                                .ConfigureAwait(false);

                            lastStatementsEntity.IsAccepted = false;
                            await _context.SaveChangesAsync().ConfigureAwait(false);
                        }

                        isSomethingChanged = true;
                    }
                }
            }
        }

        private async Task SeedUniversitiesAsync()
        {
            if (await _context.Universities.AnyAsync().ConfigureAwait(false))
                return;

            var universities = new List<UniversityEntity>
            {
                new UniversityEntity
                {
                    Region = new RegionEntity
                    {
                        Name = "Тернопільска обл."
                    },
                    FullName = "Тернопільський Національний Економічний Університет",
                    LevelOfAccreditation = "Вищий навчальний заклад України IV-го рівня акредитації",
                    UniversitySpecialities = new List<UniversitySpecialityEntity>
                    {
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Комп’ютерна інженерія",
                                Code = "123",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій",
                                CountOfPlaces = 20
                            }
                        },
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Програмна інженерія",
                                Code = "124",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій",
                                CountOfPlaces = 20
                            }
                        }
                    },
                    Ownership = "Державна",
                    Chief = "Крисоватий Андрій Ігорович",
                    Subordination = "Мінестерство освіти і науки, молоді та спорту України",
                    PostIndex = "46020",
                    Address = "вул. Львівська 11",
                    Phone = "(0352) 47 50 63",
                    Email = "info@tneu.edu.ua",
                    Site = "www.tneu.edu.ua"
                },
                new UniversityEntity
                {
                    Region = new RegionEntity
                    {
                        Name = "Львівська обл."
                    },
                    FullName = "Національний університет Львівська політехніка",
                    LevelOfAccreditation = "Вищий навчальний заклад України IV-го рівня акредитації",
                    UniversitySpecialities = new List<UniversitySpecialityEntity>
                    {
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Комп’ютерна інженерія",
                                Code = "123",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій",
                                CountOfPlaces = 20
                            }
                        },
                        new UniversitySpecialityEntity
                        {
                            Specialty = new SpecialityEntity
                            {
                                Name = "Програмна інженерія",
                                Code = "124",
                                SubjectScores = "Українська мова та література (ЗНО), k=0,25 ,балмін 100\n\n Математика (ЗНО), k=0,4 ,балмін 100\n\nФізика(ЗНО), k=0,35 ,балмін 100\n\nАнглійська мова (ЗНО), k=0,35 ,балмін 100\n\nСередній бал документа про освіту, k=0",
                                Faculty = "Факультет комп'ютерних інформаційних технологій",
                                CountOfPlaces = 20
                            }
                        }
                    },
                    Ownership = "Державна",
                    Chief = "Бобало Юрій Ярославович",
                    Subordination = "Мінестерство освіти і науки, молоді та спорту України",
                    PostIndex = "79013",
                    Address = "вул. Степана Бандери 12",
                    Phone = "(032) 258-22-19",
                    Email = "web@lpnu.ua",
                    Site = "lp.edu.ua/"

                }
            };

            await _context.Universities.AddRangeAsync(universities).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task SeedEntrantsAsync()
        {
            if (await _context.Entrants.AnyAsync().ConfigureAwait(false))
                return;

            var entrants = new List<EntrantEntity>
            {
                new EntrantEntity
                {
                    FirstName = "В.",
                    LastName = "Скотніцький",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 7.2f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 131f,
                        SecondMark = 142f,
                        ThirdMark = 171f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 151.358F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 151.358F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 151.358F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 151.358F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "А.",
                    LastName = "Капран",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10.6f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 172f,
                        SecondMark = 187f,
                        ThirdMark = 163f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 178.704F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 178.704F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 178.704F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 178.704F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "В.",
                    LastName = "Шкрібинець",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 9.8f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 170f,
                        SecondMark = 180f,
                        ThirdMark = 174f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 178.500F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 178.500F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 178.500F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 178.500F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Б.",
                    LastName = "Лагода",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 175f,
                        SecondMark = 171f,
                        ThirdMark = 151f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 173.851F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 173.851F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 173.851F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 173.851F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Ю.",
                    LastName = "Кріль",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 9.8f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 152f,
                        SecondMark = 183f,
                        ThirdMark = 157f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 168.708F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 168.708F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 168.708F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 168.708F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "А.",
                    LastName = "Гарасівка",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 9.2f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 164f,
                        SecondMark = 168f,
                        ThirdMark = 162f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 168.708F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 168.708F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 168.708F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 168.708F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Б.",
                    LastName = "Глинський",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 8.8f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 157f,
                        SecondMark = 180f,
                        ThirdMark = 150f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 166.158F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 166.158F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 166.158F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 166.158F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "А.",
                    LastName = "іванов",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 9.2f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 140f,
                        SecondMark = 188f,
                        ThirdMark = 147f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 162.894F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 162.894F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 162.894F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 162.894F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "С.",
                    LastName = "Козловський",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 9.7f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 144f,
                        SecondMark = 164f,
                        ThirdMark = 154f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 162.615F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 162.615F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 162.615F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 162.615F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "В.",
                    LastName = "Козак",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 9.1f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 144f,
                        SecondMark = 160f,
                        ThirdMark = 160f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 162.486F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 162.486F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 162.486F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 162.486F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Р.",
                    LastName = "Бортник",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 8.6f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 136f,
                        SecondMark = 162f,
                        ThirdMark = 167f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 159.222F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 159.222F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 159.222F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 159.222F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "В.",
                    LastName = "Лавренчук",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 8.8f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 133f,
                        SecondMark = 173f,
                        ThirdMark = 153f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 157.590F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 157.590F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 157.590F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 157.590F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Т.",
                    LastName = "Гук",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10.1f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 142f,
                        SecondMark = 170f,
                        ThirdMark = 138f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 156.162F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 156.162F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 156.162F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 156.162F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "А.",
                    LastName = "Кобозєв",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 9.4f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 153f,
                        SecondMark = 143f,
                        ThirdMark = 151f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 154.530F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 154.530F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 154.530F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 154.530F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "А.",
                    LastName = "Коваль",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 9.2f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 136f,
                        SecondMark = 139f,
                        ThirdMark = 150f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 150.546F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 150.546F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 150.546F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 150.546F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Ю.",
                    LastName = "Заяць",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 136f,
                        SecondMark = 182f,
                        ThirdMark = 104f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 147.492F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 147.492F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 147.492F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 147.492F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Т.",
                    LastName = "Шнуренко",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 8.3f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 136f,
                        SecondMark = 151f,
                        ThirdMark = 130f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 144.228F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 144.228F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 144.228F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 144.228F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Ю.",
                    LastName = "Репетило",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 7.8f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 129f,
                        SecondMark = 135f,
                        ThirdMark = 140f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 139.740F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 139.740F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 139.740F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 139.740F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Н.",
                    LastName = "Микитюк",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 9f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 119f,
                        SecondMark = 142f,
                        ThirdMark = 130f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 139.726F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 139.726F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 139.726F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 139.726F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "В.",
                    LastName = "Олійник",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 8f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 107f,
                        SecondMark = 148f,
                        ThirdMark = 146f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 139.026F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 139.026F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 139.026F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 139.026F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Р.",
                    LastName = "Лаута",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 8.1f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 117f,
                        SecondMark = 138f,
                        ThirdMark = 140f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 137.292F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 137.292F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 137.292F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 137.292F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "А.",
                    LastName = "Німчаковський",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 8.7f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 107f,
                        SecondMark = 142f,
                        ThirdMark = 120f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 132.547F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 132.547F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 132.547F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 132.547F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "А.",
                    LastName = "Паламарчук",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 8.3f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 127f,
                        SecondMark = 127f,
                        ThirdMark = 102f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 125.562F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 125.562F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 125.562F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 125.562F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Р.",
                    LastName = "Бубало",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 6.5f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 107f,
                        SecondMark = 123f,
                        ThirdMark = 122f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 122.502F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 122.502F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 122.502F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 122.502F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        }
                    }
                },
                                 new EntrantEntity
                {
                    FirstName = "Ю.",
                    LastName = "Чипак",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10.4f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 182f,
                        SecondMark = 192f,
                        ThirdMark = 185f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 189.669F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 189.669F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 189.669F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 189.669F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "А.",
                    LastName = "Павлючик",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10.3f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 170f,
                        SecondMark = 169f,
                        ThirdMark = 179f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 176.358F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 176.358F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 176.358F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 176.358F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Д.",
                    LastName = "Аксьонов",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10.6f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 182f,
                        SecondMark = 165f,
                        ThirdMark = 166f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 175.593F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 175.593F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 175.593F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 175.593F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "і.",
                    LastName = "Гапончук",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 8.8f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 172f,
                        SecondMark = 167f,
                        ThirdMark = 168f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 172.737F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 172.737F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 172.737F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 172.737F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Д.",
                    LastName = "Кононова",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 8.9f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 166f,
                        SecondMark = 148f,
                        ThirdMark = 183f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 170.799F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 170.799F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 170.799F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 170.799F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "В.",
                    LastName = "Поленчук",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 9.2f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 140f,
                        SecondMark = 148f,
                        ThirdMark = 128f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 145.448F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 145.448F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 145.448F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 145.448F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "М.",
                    LastName = "Тракало",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.3f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 183f,
                        SecondMark = 186f,
                        ThirdMark = 199f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "А.",
                    LastName = "Калимон",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.5f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 197f,
                        SecondMark = 198f,
                        ThirdMark = 194f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Б.",
                    LastName = "Яріш",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10.7f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 191f,
                        SecondMark = 190f,
                        ThirdMark = 190f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Я.",
                    LastName = "Керніцький",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.4f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 200f,
                        SecondMark = 192f,
                        ThirdMark = 197f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "А.",
                    LastName = "Кашапов",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.2f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 193f,
                        SecondMark = 196f,
                        ThirdMark = 197f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Н.",
                    LastName = "Скоропад",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.1f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 198f,
                        SecondMark = 197f,
                        ThirdMark = 195f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "В.",
                    LastName = "Селіверстова",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.5f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 198f,
                        SecondMark = 189f,
                        ThirdMark = 187f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 200.000F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "С.",
                    LastName = "Химера",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.8f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 193f,
                        SecondMark = 197f,
                        ThirdMark = 197f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 199.487F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 199.487F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 199.487F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 199.487F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "А.",
                    LastName = "Голда",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.4f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 192f,
                        SecondMark = 197f,
                        ThirdMark = 194f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 198.314F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 198.314F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 198.314F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 198.314F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "В.",
                    LastName = "Бенюх",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10.7f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 198f,
                        SecondMark = 191f,
                        ThirdMark = 192f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 198.033F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 198.033F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 198.033F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 198.033F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Р.",
                    LastName = "Гудима",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.7f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 198f,
                        SecondMark = 197f,
                        ThirdMark = 198f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 198.033F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 198.033F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 198.033F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 198.033F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "В.",
                    LastName = "Долинський",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10.4f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 193f,
                        SecondMark = 196f,
                        ThirdMark = 193f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.931F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.931F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.931F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.931F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "і.",
                    LastName = "Бойчук",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 196f,
                        SecondMark = 189f,
                        ThirdMark = 198f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.931F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.931F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.931F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.931F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Т.",
                    LastName = "Лисак",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.4f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 194f,
                        SecondMark = 196f,
                        ThirdMark = 189f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.727F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.727F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.727F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.727F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "В.",
                    LastName = "Вєтров",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.2f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 191f,
                        SecondMark = 187f,
                        ThirdMark = 191f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.676F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.676F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.676F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.676F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Л.",
                    LastName = "іванська",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.3f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 195f,
                        SecondMark = 193f,
                        ThirdMark = 193f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.676F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.676F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.676F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.676F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "В.",
                    LastName = "Прищепа",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10.5f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 200f,
                        SecondMark = 195f,
                        ThirdMark = 181f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.370F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.370F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.370F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.370F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "В.",
                    LastName = "Заворотний",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.8f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 198f,
                        SecondMark = 184f,
                        ThirdMark = 195f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.319F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.319F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.319F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.319F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "Д.",
                    LastName = "Гопайнич",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 11.7f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 186f,
                        SecondMark = 196f,
                        ThirdMark = 186f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.156F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.156F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.156F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.156F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
                 new EntrantEntity
                {
                    FirstName = "М.",
                    LastName = "Коток",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    CertificateOfSecondaryEducation = new CertificateOfSecondaryEducationEntity()
                    {
                        YearOfIssue = new DateTime(2016, 6, 25),
                        SerialNumber = "1239485020394",
                        AverageMark = 10.8f,
                        FullNameOfTheEducationalInstitution = "Дубенська гімназія  №2"
                    },
                    CertificateOfTesting = new CertificateOfTestingEntity
                    {
                        YearOfIssue = new DateTime(2016, 7, 25),
                        SerialNumber = "1239483929182",
                        FirstMark = 197f,
                        SecondMark = 192f,
                        ThirdMark = 189f,
                        FourthMark = 0f,
                        FirstSubject = "Математика (ЗНО)",
                        SecondSubject = "Українська мова та література (ЗНО)",
                        ThirdSubject = "історія України (ЗНО)",
                        FourthSubject = ""
                    },
                    Statements = new List<StatementEntity>
                    {
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.115F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 3,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.115F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Національний університет Львівська політехніка")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 4,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.115F, SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Програмна інженерія").SpecialtyId,
                            Priority = 1,
                            IsAccepted = false
                        },
                        new StatementEntity
                        {
                            ExtraScore = 0,
                            TotalScore = 197.115F,
                            SpecialityId = (await _context.Universities.Include(x=> x.UniversitySpecialities).ThenInclude(x=> x.Specialty)
                                .FirstOrDefaultAsync(
                                    x => x.FullName == "Тернопільський Національний Економічний Університет")
                                .ConfigureAwait(false)).UniversitySpecialities.First(x=> x.Specialty.Name == "Комп’ютерна інженерія").SpecialtyId,
                            Priority = 2,
                            IsAccepted = false
                        }
                    }
                },
            };

            await _context.Entrants.AddRangeAsync(entrants).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await CalculateRatings().ConfigureAwait(false);
            stopWatch.Stop();
            Console.WriteLine(stopWatch.Elapsed);
        }

        private async Task SeedUsersAsync()
        {
            if(await _context.Users.AnyAsync().ConfigureAwait(false))
                return;

            var users = new List<UserEntity>
            {
                new UserEntity
                {
                    FirstName = "Nazar",
                    LastName = "Melnyk",
                    Birthday = new DateTimeOffset(1998, 6, 17, 0, 0, 0, new TimeSpan()),
                    Email = "88nazar88@gmail.com",
                    Password = _cryptoProvider.EncodeValue("Qwerty123*"),
                    Phone = "+380660367890"
                },
                new UserEntity
                {
                    FirstName = "Anastasi",
                    LastName = "Sokolovska",
                    Birthday = new DateTimeOffset(1997, 9, 4, 0, 0, 0, new TimeSpan()),
                    Email = "sokolovskagmail.com",
                    Password = _cryptoProvider.EncodeValue("Qwerty123*"),
                    Phone = "+380672385429"
                },
                new UserEntity
                {
                    FirstName = "Max",
                    LastName = "Lukashyk",
                    Birthday = new DateTimeOffset(1998, 5, 21, 0, 0, 0, new TimeSpan()),
                    Email = "maxkvf@gmail.com",
                    Password = _cryptoProvider.EncodeValue("Qwerty123*"),
                    Phone = "+380664578432"
                }
            };

            await _context.Users.AddRangeAsync(users).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
