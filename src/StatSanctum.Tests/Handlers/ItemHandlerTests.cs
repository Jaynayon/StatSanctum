using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using StatSanctum.Entities;
using StatSanctum.Handlers;
using StatSanctum.Repositories;

namespace StatSanctum.Tests.Handlers
{
    public class ItemHandlerTests
    {
        private readonly Handler<Item> handler;
        private readonly Mock<IRepository<Item>> repositoryMock;

        public ItemHandlerTests()
        {
            repositoryMock = new Mock<IRepository<Item>>();
            handler = new Handler<Item>(repositoryMock.Object);
        }

        [Theory, AutoData]
        public async Task GetById_ReturnCorrectResponse(GetByIdQuery<Item> request)
        {
            // Arrange
            var expectedItem = new Item { ItemID = request.Id };
            repositoryMock.Setup(m => m.GetByIdAsync(request.Id))
                .ReturnsAsync(expectedItem);

            // Action
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            using(new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(expectedItem);
                repositoryMock.Verify(x =>  x.GetByIdAsync(request.Id), Times.Once());
            }
        }

        [Theory, AutoData]
        public async Task GetById_ShouldThrowKeyNotFoundException_WhenItemDoesNotExist(GetByIdQuery<Item> request)
        {
            // Arrange
            repositoryMock.Setup(m => m.GetByIdAsync(request.Id))
                         .ThrowsAsync(new KeyNotFoundException());

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(request, CancellationToken.None));

            //await handler.Invoking(async h =>  await h.Handle(request, CancellationToken.None))
            //    .Should()
            //    .ThrowAsync<KeyNotFoundException>();

            // Verify
            repositoryMock.Verify(x => x.GetByIdAsync(request.Id), Times.Once());
        }
    }
}
