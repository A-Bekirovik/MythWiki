using System;
using UnitTest.FakeDAL;
using MythWikiBusiness.Services;
using MythWikiBusiness.ErrorHandling;
using MythWikiBusiness.DTO;
using MythWikiBusiness.Models;
using MythWikiBusiness.IRepository;

namespace MythWikiTests
{
    [TestClass]
    public class SubjectUnitTests
    {
        private ISubjectRepo _subjectRepository;
        private SubjectService _subjectService;

        [TestInitialize]
        public void Setup()
        {
            _subjectRepository = new FakeSubjectRepo();
            _subjectService = new SubjectService(_subjectRepository);
        }

        [TestMethod]
        public void CreateSubject_ShouldReturnSubject()
        {
            // Arrange
            string title = "Sample Title";
            string text = "Sample Text";
            int authorID = 1;
            string imagelink = "https://partyflock.nl/images/party/449316_539x303_650691/Boef.jpg";

            // Act
            var result = _subjectService.CreateSubject(title, text, authorID, imagelink);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Subject));
            Assert.AreEqual(title, result.Title);
            Assert.AreEqual(text, result.Text);
            Assert.AreEqual(authorID, result.AuthorID);
            Assert.AreEqual(imagelink, result.Image);
        }

        [TestMethod]
        public void CreateSubject_ShouldReturnError()
        {
            // Arrange
            string title = null;
            string text = "Sample Text";
            int authorID = 1;
            string imagelink = "https://partyflock.nl/images/party/449316_539x303_650691/Boef.jpg";

            // Act
            var result = _subjectService.CreateSubject(title, text, authorID, imagelink);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Subject));
            Assert.AreEqual(title, result.Title);
            Assert.AreEqual(text, result.Text);
            Assert.AreEqual(authorID, result.AuthorID);
            Assert.AreEqual(imagelink, result.Image);
        }
    }
}
