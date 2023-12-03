function renderGauge(curentValue) {
    var myConfig = {
        type: "gauge",
        globals: {
            fontSize: 17
        },
        plotarea: {
            marginTop: 80
        },
        plot: {
            size: '100%',
            valueBox: {
                placement: 'center',
                text: '%v', //default
                fontSize: 35,
                rules: [
                    {
                        rule: '%v >= 2154',
                        text: '%v<br>Кришна — Бог!'
                    },
                    {
                        rule: '%v < 2154 && %v > 1107',
                        text: '%v<br>МОЛОДЦЫ!'
                    },
                    {
                        rule: '%v < 1108 && %v > 1000',
                        text: '%v<br>Ещё чуть-чуть'
                    },
                    {
                        rule: '%v <  1000',
                        text: '%v<br>Маловато'
                    }
                ]
            }
        },
        tooltip: {
            borderRadius: 5
        },
        scaleR: {
            aperture: 300,
            minValue: 0,
            maxValue: 3000,
            step: 10,
            center: {
                visible: false
            },
            tick: {
                visible: false
            },
            item: {
                visible: true,
                offsetR: 0,
                rules: [{
                    rule: '%i == 9',
                    offsetX: 15
                }]
            },
            ring: {
                size: 70,
                rules: [
                    {
                        rule: '%v <= 1108',
                        backgroundColor: '#E53935'
                    },
                    {
                        rule: '%v >= 1108 && %v < 2154',
                        backgroundColor: '#2ee7e7'
                    },
                    {
                        rule: '%v >= 2154',
                        backgroundColor: '#05b21a'
                    }
                ]
            }
        },
        series: [{
            values: [curentValue], // starting value
            backgroundColor: 'black',
            indicator: [10, 10, 10, 10, 0.75],
            animation: {
                effect: 2,
                method: 1,
                sequence: 4,
                speed: 900
            },
        }]
    };

    zingchart.render({
        id: 'myChart',
        data: myConfig,
        height: 500,
        width: '100%'
    });
}