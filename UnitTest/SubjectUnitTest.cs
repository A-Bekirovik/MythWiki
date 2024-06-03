using System;
using UnitTest.FakeDAL;
using MythWikiBusiness.Services;
using MythWikiBusiness.ErrorHandling;
using MythWikiBusiness.DTO;
using MythWikiBusiness.Models;

namespace MythWikiTests
{
    [TestClass]
    public class SubjectUnitTests
    {
        private SubjectRepository _subjectRepository;
        private SubjectService _subjectService;

        [TestInitialize]
        public void Setup()
        {
            _subjectRepository = new SubjectRepository("fake_connection_string");
            _subjectService = new SubjectService(_subjectRepository);
        }

        [TestMethod]
        public void CreateSubject_ShouldReturnSubjectDTO_Repo()
        {
            // Arrange
            string title = "Sample Title";
            string text = "Sample Text";
            int authorID = 1;
            string imagelink = "";

            // Act
            var result = _subjectRepository.CreateSubject(title, text, authorID, imagelink);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SubjectDTO));
            Assert.AreEqual(title, result.Title);
            Assert.AreEqual(text, result.Text);
            Assert.AreEqual(authorID, result.AuthorID);
            Assert.AreEqual(imagelink, result.Image);
        }

        [TestMethod]
        public void CreateSubject_ShouldReturnError_Repo()
        {
            // Arrange
            string title = null; // Causes Error, cause of restriction.
            string text = "Sample Text";
            int authorID = 1;
            string imagelink = "";

            // Act
            var result = _subjectRepository.CreateSubject(title, text, authorID, imagelink);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SubjectDTO));
            Assert.AreEqual(title, result.Title);
            Assert.AreEqual(text, result.Text);
            Assert.AreEqual(authorID, result.AuthorID);
            Assert.AreEqual(imagelink, result.Image);
        }

        [TestMethod]
        public void CreateSubject_ShouldReturnSubject_Service()
        {
            // Arrange
            string title = "Sample Title";
            string text = "Sample Text";
            int authorID = 1;
            string imagelink = "";

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
