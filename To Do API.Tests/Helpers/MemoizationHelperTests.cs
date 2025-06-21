using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using To_Do_API.Application.Helpers;

namespace To_Do_API.Tests.Helpers
{
    public class MemoizationHelperTests
    {
        [Theory]
        [InlineData(0, 0, 0.0)]
        [InlineData(3, 6, 50.0)]
        [InlineData(5, 10, 50.0)]
        [InlineData(7, 10, 70.0)]
        [InlineData(10, 10, 100.0)]
        public void CalcularPorcentajeCompletado_RetornaPorcentajeCorrecto(int tareasCompletadas, int totalTareas, double porcentajeEsperado)
        {
            //Act
            var result = MemoizationHelper.CalculateCompletionPercentage(tareasCompletadas, totalTareas);

            //Assert
            Assert.Equal(porcentajeEsperado, result);
        }
    }
}
