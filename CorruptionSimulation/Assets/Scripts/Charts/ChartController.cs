using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E2C;

public class ChartController : MonoBehaviour
{
    private int day = 0;
    [SerializeField] E2Chart myChart;
    private E2ChartData.Series series1;
    private E2ChartData.Series series2;
    private E2ChartData.Series series3;
    void Start()
    {
        //Add chart component
        myChart.chartType = E2Chart.ChartType.LineChart;

        //add chart options
        //myChart.chartOptions = myChart.gameObject.AddComponent<E2ChartOptions>();
        myChart.chartOptions = myChart.gameObject.GetComponent<E2ChartOptions>();
       /* myChart.chartOptions.title.enableTitle = true;
        myChart.chartOptions.title.enableSubTitle = false;
        myChart.chartOptions.yAxis.enableTitle = true;
        myChart.chartOptions.label.enable = true;
        myChart.chartOptions.legend.enable = true;
        myChart.chartOptions.chartStyles.lineChart.enableShade = true;
        myChart.chartOptions.plotOptions.columnStacking = E2ChartOptions.ColumnStacking.Normal;
        myChart.chartOptions.chartStyles.barChart.barWidth = 15.0f;
        myChart.chartOptions.plotOptions.mouseTracking = E2ChartOptions.MouseTracking.None;*/

        //add chart data
        myChart.chartData = myChart.gameObject.AddComponent<E2ChartData>();
        myChart.chartData.title = "Комуняшки";
        myChart.chartData.yAxisTitle = "Популяция";
        //myChart.chartData.categoriesX = new List<string> { "День " + day}; //set categories
        myChart.chartData.categoriesX = new List<string> { day.ToString() }; //set categories

        //create new series
        series1 = new E2ChartData.Series();
        series1.name = "Альтруисты";
        series1.dataY = new List<float>();
        series1.dataY.Add(0);

        series2 = new E2ChartData.Series();
        series2.name = "Эгоисты";
        series2.dataY = new List<float>();
        series2.dataY.Add(0);

        series3 = new E2ChartData.Series();
        series3.name = "Коррупционеры";
        series3.dataY = new List<float>();
        series3.dataY.Add(0);

        //add series into series list
        myChart.chartData.series = new List<E2ChartData.Series>();
        myChart.chartData.series.Add(series1);
        myChart.chartData.series.Add(series2);
        myChart.chartData.series.Add(series3);
        //myChart.chartData.series.Add(series2);

        //update chart
        myChart.UpdateChart();
    }

    public void AddDataToChart(int altruistPopulationAmount, int egoistPopulationAmount, int corruptedPopulationAmount, int interationIndex)
    {
        day = interationIndex;
        series1.dataY.Add(altruistPopulationAmount);
        series2.dataY.Add(egoistPopulationAmount);
        series3.dataY.Add(corruptedPopulationAmount);
        //myChart.chartData.categoriesX.Add("День" + day);

        myChart.chartData.categoriesX.Add(day.ToString());

        myChart.UpdateChart();
    }

}
