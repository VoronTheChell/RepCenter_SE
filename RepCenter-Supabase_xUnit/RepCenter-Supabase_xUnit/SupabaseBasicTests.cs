using FluentAssertions;
using Supabase.Postgrest;

namespace RepCenter_Supabase_xUnit
{
    public class SupabaseBasicTests
    {
        private readonly SupabaseTestService _service;

        public SupabaseBasicTests()
        {
            _service = new SupabaseTestService();
            _service.InitializeAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task CanConnectToSupabase()
        {
            var userTable = await _service.Client.From<Register>().Get();
            userTable.Models.Should().NotBeNull();
        }

        [Fact]
        public async Task CanInsertUser()
        {
            var newUser = new Register
            {
                LoginUser = "test_login",
                PasswordUser = "test_password",
                SubjectUser = "Math",
                StatusUser = "admin"
            };

            var result = await _service.Client.From<Register>().Insert(newUser);
            result.Models.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CanRetrieveUserByLogin()
        {
            var users = await _service.Client
                .From<Register>()
                .Filter("login_user", Constants.Operator.Equals, "test_login")
                .Get();

            users.Models.FirstOrDefault()?.LoginUser.Should().Be("test_login");
        }

        [Fact]
        public async Task CanDeleteUser()
        {
            var users = await _service.Client
                .From<Register>()
                .Filter("login_user", Constants.Operator.Equals, "test_login")
                .Get();

            var user = users.Models.FirstOrDefault();
            if (user != null)
            {
                var result = await _service.Client.From<Register>().Delete(user);
                result.Models.Count.Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task CanDeleteSubject()
        {
            var subjects = await _service.Client
                .From<Pridments>()
                .Filter("name_pridment", Constants.Operator.Equals, "TestSubject")
                .Get();

            var subject = subjects.Models.FirstOrDefault();
            if (subject != null)
            {
                var result = await _service.Client.From<Pridments>().Delete(subject);
                result.Models.Count.Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task CanInsertStudent()
        {
            var student = new Student
            {
                FIO = "Test Student",
                DateBirth = DateTime.Now.ToString("dd.MM.yyyy"),
                NumberPhone = "+79001234567",
                EmailAdress = "test@student.com",
                Predmet = "Math"
            };

            var result = await _service.Client.From<Student>().Insert(student);
            result.Models.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CanDeleteStudent()
        {
            var students = await _service.Client
                .From<Student>()
                .Filter("fio", Constants.Operator.Equals, "Test Student")
                .Get();

            var student = students.Models.FirstOrDefault();
            if (student != null)
            {
                var result = await _service.Client.From<Student>().Delete(student);
                result.Models.Count.Should().BeGreaterThan(0);
            }
        }
    }
}
