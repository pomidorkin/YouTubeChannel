using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E2C;

public class PiePopulationChart : MonoBehaviour
{
    [SerializeField] E2Chart myChart;
    private E2ChartData.Series series1;
    // Start is called before the first frame update
    void Start()
    {
        //Add chart component
        myChart.chartType = E2Chart.ChartType.PieChart;

        //add chart options
        myChart.chartOptions = myChart.gameObject.GetComponent<E2ChartOptions>();

        //add chart data
        myChart.chartData = myChart.gameObject.AddComponent<E2ChartData>();
        myChart.chartData.title = "Процент среди\n популяции";

        //create new series
        series1 = new E2ChartData.Series();
        series1.name = "Альтруисты";
        series1.dataName = new List<string>();
        series1.dataName.Add("Простаки");
        series1.dataName.Add("Эгоисты");
        series1.dataName.Add("Злопамятные");
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

    public void AddDataToChart(int individualistPopulationAmount, int egoistPopulationAmount, int corruptedPopulationAmount)
    {
        series1.dataY.Clear();
        series1.dataY.Add(individualistPopulationAmount);
        series1.dataY.Add(egoistPopulationAmount);
        series1.dataY.Add(corruptedPopulationAmount);

        myChart.UpdateChart();
    }
}
