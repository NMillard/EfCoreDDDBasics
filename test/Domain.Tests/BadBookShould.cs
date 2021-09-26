using Domain.BadExamples;
using Xunit;

namespace Domain.Tests {
    public class BadBookShould {
        [Fact]
        public void CreateNewInvalidBook() {
            var book = new BadBook();
            
            Assert.NotNull(book);
        }
    }
}