enum LogLevel { INFO = "INFO", WARNING = "WARNING", ERROR = "ERROR" };

type LogEntry = [
    timestamp: Date,
    level: LogLevel,
    message: string,
]


function logEvent(event: LogEntry) {
    const [timestamp, level, message] = event;
    console.log(`[${timestamp}] [${level}]: ${message}`);
}

logEvent([new Date(), LogLevel.INFO, "Система запущена"]);