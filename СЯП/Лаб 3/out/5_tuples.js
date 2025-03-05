var LogLevel;
(function (LogLevel) {
    LogLevel["INFO"] = "INFO";
    LogLevel["WARNING"] = "WARNING";
    LogLevel["ERROR"] = "ERROR";
})(LogLevel || (LogLevel = {}));
;
function logEvent(event) {
    const [timestamp, level, message] = event;
    console.log(`[${timestamp}] [${level}]: ${message}`);
}
logEvent([new Date(), LogLevel.INFO, "Система запущена"]);
//# sourceMappingURL=5_tuples.js.map