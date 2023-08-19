using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E2C;

public class PopulationThresholdChart : MonoBehaviour
{
    private int iteration = 0;
    [SerializeField] E2Chart myChart;
    private E2ChartData.Series series1;
    private E2ChartData.Series series2;
    void Start()
    {
        //Add chart component
        myChart.chartType = E2Chart.ChartType.LineChart;

        //add chart options
        myChart.chartOptions = myChart.gameObject.GetComponent<E2ChartOptions>();

        //add chart data
        myChart.chartData = myChart.gameObject.AddComponent<E2ChartData>();
        myChart.chartData.title = "Популяция";
        myChart.chartData.yAxisTitle = "Y Axis Title";
        myChart.chartData.categoriesX = new List<string> { "" }; //set categories

        //create new series
        series1 = new E2ChartData.Series();
        series1.name = "Красные";
        series1.dataY = new List<float>();
        series1.dataY.Add(0);

        series2 = new E2ChartData.Series();
        series2.name = "Зеленые";
        series2.dataY = new List<float>();
        series2.dataY.Add(0);

        //add series into series list
        myChart.chartData.series = new List<E2ChartData.Series>();
        myChart.chartData.series.Add(series1);
        myChart.chartData.series.Add(series2);
        //myChart.chartData.series.Add(series2);

        //update chart
        myChart.UpdateChart();
    }

    public void AddDataToChart(int redPopulation, int greenPopulation)
    {
        if (series1 != null && series2 != null)
        {
            iteration += 5;
            series1.dataY.Add(redPopulation);
            series2.dataY.Add(greenPopulation);
            myChart.chartData.categoriesX.Add(iteration.ToString());

            myChart.UpdateChart();
        }
    }
}
