using System;
using Application.Controllers;
using Xunit;

namespace Application.UnitTests.Controllers
{
    public class NoteControllerTests
    {
        [Fact]
        public void NullMediator_Ctor_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>((() => new NoteController(null)));
        }
    }
}
