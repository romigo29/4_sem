interface IReport {
    title: string;
    content: string;
    generate(): string;

}

class HTMLReport implements IReport {

    constructor(public title: string, public content: string) {
        this.title = title;
        this.content = content;
    }

    generate() {
        return `<h1>${this.title}</h1><p>${this.content}</p>`;
    }
}

class JSONReport implements IReport {

    constructor(public title: string, public content: string) {
        this.title = title;
        this.content = content;
    }

    generate() {
        return `{ title: "${this.title}", content: "${this.content}" }`;
    }
}

const report1 = new HTMLReport("Отчет 1", "Содержание отчета");
console.log(report1.generate());

const report2 = new JSONReport("Отчет 2", "Содержание отчета");
console.log(report2.generate());