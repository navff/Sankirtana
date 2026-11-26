const chart = document.querySelector(".daily-chart");

if (chart) {
    const canvas = chart.querySelector("canvas");
    const tooltip = chart.querySelector(".daily-tooltip");
    const context = canvas.getContext("2d");
    const counts = chart.dataset.counts.split(",").map(Number);
    const days = Number(chart.dataset.days);
    const numberFormat = new Intl.NumberFormat("ru-RU");
    const dateFormat = new Intl.DateTimeFormat("ru-RU", {
        day: "numeric",
        month: "long",
        timeZone: "UTC"
    });

    const targetStep = Math.max(1, Math.max(...counts) / 4);
    const magnitude = 10 ** Math.floor(Math.log10(targetStep));
    const step = Math.ceil(magnitude * [1, 2, 2.5, 5, 10].find(value => value * magnitude >= targetStep));
    const ceiling = Math.ceil(Math.max(...counts) / step) * step;

    let points = [];
    let activeDay = -1;
    let bounds;

    function drawChart() {
        const { width, height } = canvas.getBoundingClientRect();
        if (width === 0 || height === 0) return;

        const pixelRatio = window.devicePixelRatio || 1;
        canvas.width = Math.round(width * pixelRatio);
        canvas.height = Math.round(height * pixelRatio);
        context.setTransform(pixelRatio, 0, 0, pixelRatio, 0, 0);
        context.font = "11px system-ui, sans-serif";

        const left = context.measureText(numberFormat.format(ceiling)).width + 11;
        const right = width - 12;
        const top = 12;
        const bottom = height - 22;
        const x = day => left + (right - left) * day / (counts.length - 1);
        const y = value => bottom - (bottom - top) * value / ceiling;

        bounds = { left, right, top, bottom };
        points = counts.slice(0, days).map((count, day) => [x(day), y(count)]);
        context.fillStyle = "#52685e";
        context.strokeStyle = "#dbe5dc";
        context.textAlign = "right";
        context.textBaseline = "middle";

        for (let value = 0; value <= ceiling; value += step) {
            const gridY = y(value);
            context.beginPath();
            context.moveTo(left, gridY);
            context.lineTo(right, gridY);
            context.stroke();
            context.fillText(numberFormat.format(value), left - 7, gridY);
        }

        context.textAlign = "center";
        const lastDay = counts.length - 1;
        const tickDays = width < 390 ? [0, 9, 19, lastDay] : [0, 4, 9, 14, 19, 24, lastDay];
        for (const day of tickDays) {
            context.fillText(String(day + 1), x(day), bottom + 14);
        }

        const area = new Path2D();
        area.moveTo(left, bottom);
        const line = new Path2D();
        points.forEach(([pointX, pointY], index) => {
            area.lineTo(pointX, pointY);
            if (index === 0) line.moveTo(pointX, pointY);
            else line.lineTo(pointX, pointY);
        });
        const [lastX, lastY] = points[points.length - 1];
        area.lineTo(lastX, bottom);
        area.closePath();
        context.fillStyle = "#e2efe3";
        context.fill(area);

        context.strokeStyle = "#286d59";
        context.lineWidth = 2.4;
        context.lineCap = "round";
        context.lineJoin = "round";
        context.stroke(line);

        const [dotX, dotY] = points[activeDay] ?? [lastX, lastY];
        if (activeDay >= 0) {
            context.strokeStyle = "#75988a";
            context.lineWidth = 1;
            context.beginPath();
            context.moveTo(dotX, top);
            context.lineTo(dotX, bottom);
            context.stroke();
        }

        context.fillStyle = "#286d59";
        context.beginPath();
        context.arc(dotX, dotY, activeDay >= 0 ? 4.5 : 3.5, 0, 2 * Math.PI);
        context.fill();
    }

    chart.addEventListener("pointermove", event => {
        if (!bounds) return;
        const { left, right, top, bottom } = bounds;
        const rect = canvas.getBoundingClientRect();
        const x = event.clientX - rect.left;
        const y = event.clientY - rect.top;
        const day = Math.round((x - left) * (counts.length - 1) / (right - left));
        const nextDay = x >= left - 6 && y >= top - 6 && y <= bottom && day >= 0 && day < days ? day : -1;
        if (activeDay === nextDay) return;

        activeDay = nextDay;
        drawChart();
        tooltip.hidden = activeDay < 0;
        if (activeDay >= 0) {
            const date = new Date(`${chart.dataset.month}-${String(activeDay + 1).padStart(2, "0")}T00:00:00Z`);
            tooltip.textContent = `${dateFormat.format(date)} · ${numberFormat.format(counts[activeDay])} экз.`;
            const tooltipX = points[activeDay][0] - tooltip.offsetWidth / 2;
            tooltip.style.left = `${Math.max(0, Math.min(rect.width - tooltip.offsetWidth, tooltipX))}px`;
        }
    });

    chart.addEventListener("pointerleave", () => {
        activeDay = -1;
        tooltip.hidden = true;
        drawChart();
    });

    new ResizeObserver(() => {
        activeDay = -1;
        tooltip.hidden = true;
        drawChart();
    }).observe(chart);
}
