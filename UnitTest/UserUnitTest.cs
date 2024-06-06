using System;
using UnitTest.FakeDAL;
using MythWikiBusiness.Services;
using MythWikiBusiness.ErrorHandling;
using MythWikiBusiness.DTO;
using MythWikiBusiness.Models;
using MythWikiBusiness.IRepository;
using Org.BouncyCastle.Utilities;

namespace MythWikiTests
{
    [TestClass]
    public class UserUnitTests
    {
        private FakeUserRepo _userRepository;
        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            _userRepository = new FakeUserRepo();
            _userService = new UserService(_userRepository);
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

            // Assert Hier error aangeven 
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Subject));
            Assert.AreEqual(title, result.Title);
            Assert.AreEqual(text, result.Text);
            Assert.AreEqual(authorID, result.AuthorID);
            Assert.AreEqual(imagelink, result.Image);
        }


        [TestMethod]
        public void GetAllSubjects_ShouldReturnList()
        {            
            // Act
            var result = _subjectService.GetAllSubjects();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GenerateNewSubjectID_ShouldReturnNextID()
        {
            // Act
            var result = _subjectRepository.GenerateNewSubjectID();

            // Assert
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void EditSubject_ShouldReturnTrue_WhenSubjectExists()
        {
            // Arrange
            var subjectDTO = new SubjectDTO
            {
                SubjectID = 1,
                Title = "Updated Title",
                Text = "Updated Text",
                EditorID = 1,
                Image = "updated_image.jpg",
                Date = DateTime.Now
            };

            // Act
            var result = _subjectService.EditSubject(subjectDTO);

            // Assert
            var updatedSubject = _subjectRepository.GetAllSubjects().Find(s => s.SubjectID == subjectDTO.SubjectID);
            Assert.IsNotNull(updatedSubject);
            Assert.AreEqual("Updated Title", updatedSubject.Title);
            Assert.AreEqual("Updated Text", updatedSubject.Text);
            Assert.AreEqual("updated_image.jpg", updatedSubject.Image);
            Assert.AreEqual(subjectDTO.EditorID, updatedSubject.EditorID);
            Assert.AreEqual(subjectDTO.Date, updatedSubject.Date);
        }

        [TestMethod]
        public void DeleteSubject_ShouldReturnTrue_WhenSubjectExists()
        {
            // Arrange
            int subjectID = 1;

            // Act
            var result = _subjectService.DeleteSubject(subjectID);

            // Assert
            Assert.IsTrue(result);
            var subject = _subjectRepository.GetAllSubjects().FirstOrDefault(s => s.SubjectID == subjectID);
            Assert.IsNull(subject);
        }

        [TestMethod]
        public void GetSubjectById_ShouldReturnSubject_WhenSubjectExists()
        {
            // Arrange
            int subjectID = 1;

            // Act
            var result = _subjectService.GetSubjectById(subjectID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(subjectID, result.SubjectID);	   
        }
    }
}
