using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E2C;

public class PieOpinionChart : MonoBehaviour
{
    [SerializeField] E2Chart myChart;
    private E2ChartData.Series series1;
    // Start is called before the first frame update
    void Start()
    {
        //Add chart component
        myChart.chartType = E2Chart.ChartType.PieChart;

        //add chart options
        //myChart.chartOptions = myChart.gameObject.GetComponent<E2ChartOptions>();

        //add chart data
        myChart.chartData = myChart.gameObject.AddComponent<E2ChartData>();
        myChart.chartData.title = "Мнение\n слаймов";

        //create new series
        series1 = new E2ChartData.Series();
        series1.name = "Красные";
        series1.dataName = new List<string>();
        series1.dataName.Add("Красные");
        series1.dataName.Add("Зеленые");
        series1.dataY = new List<float>();
        /*series1.dataY.Add(5);
        series1.dataY.Add(3);*/

        //add series into series list
        myChart.chartData.series = new List<E2ChartData.Series>();
        myChart.chartData.series.Add(series1);
        //myChart.chartData.series.Add(series2);

        //update chart
        myChart.UpdateChart();
    }

    public void AddDataToChart(int redPopulation, int greenPopulation)
    {
        series1.dataY.Clear();
        series1.dataY.Add(redPopulation);
        series1.dataY.Add(greenPopulation);

        myChart.UpdateChart();
    }
}
