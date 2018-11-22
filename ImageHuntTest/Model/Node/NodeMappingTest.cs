using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ImageHuntCore.Model.Node;
using ImageHuntWebServiceClient.Responses;
using NFluent;
using Xunit;

namespace ImageHuntTest.Model.Node
{
    [Collection("AutomapperFixture")]
    public class NodeMappingTest
    {
        private IMapper _mapper;

        public NodeMappingTest()
        {
            _mapper = AutoMapper.Mapper.Instance;

        }

        [Fact]
        public void MapNodeToNodeResponse()
        {
            // Arrange
            var node = new ObjectNode() {Action = "Toto",Name = "titi"};
            // Act
            var response = _mapper.Map<NodeResponse>(node);
            // Assert
            Check.That(response.Name).Equals(node.Name);
            Check.That(response.Action).Equals(node.Action);
        }
    }
}
