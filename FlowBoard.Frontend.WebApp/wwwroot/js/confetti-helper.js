window.flowConfetti = {

    burstAt: function (x, y) {
        if (typeof confetti !== "function") {
            return;
        }

        confetti({
            particleCount: 80,
            spread: 60,
            startVelocity: 35,
            origin: {
                x: x / window.innerWidth,
                y: y / window.innerHeight
            },
            colors: ["#3B82F6", "#22C55E", "#F59E0B", "#EF4444", "#8B5CF6"]
        });
    }
};