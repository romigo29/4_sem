print 'Число обработанных строк - ' + convert(varchar, @@ROWCOUNT)
print 'Версия SQL_сервера - ' + convert(varchar, @@VERSION)
print 'Системный идентификатор процесса - ' + convert(varchar, @@SPID)
print 'Код последней ошибки - ' + convert(varchar, @@ERROR)
print 'Уровень вложенности транзакции - ' + convert(varchar, @@TRANCOUNT)
print 'Результат считывания строку реузльтирующего набора - ' + convert(varchar, @@FETCH_STATUS)
print 'Уровень вложенности текущей процедуры - ' + convert(varchar, @@NESTLEVEL)

