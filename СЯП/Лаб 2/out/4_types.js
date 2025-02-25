const studentsGroup = {
    students: [
        {
            id: 1,
            name: "Serega",
            group: 6,
            marks: [
                { subject: "Math", mark: 8, done: true },
                { subject: "Phylosophy", mark: 7, done: true },
                { subject: "History", mark: 6, done: false }
            ]
        },
        {
            id: 2,
            name: "Dasha",
            group: 6,
            marks: [
                { subject: "Math", mark: 7, done: false },
                { subject: "Phylosophy", mark: 8, done: true },
                { subject: "Hystory", mark: 7, done: true }
            ]
        },
        {
            id: 3,
            name: "Andrey",
            group: 10,
            marks: [
                { subject: "Math", mark: 7, done: false },
                { subject: "Phylosophy", mark: 6, done: false },
                { subject: "Hystory", mark: 6, done: false }
            ]
        }
    ],
    mark: 8,
    group: 6,
    studentsFilter(group) {
        return this.students.filter(student => student.group == group);
    },
    marksFilter(mark) {
        return this.students.filter(student => student.marks.some(m => m.mark == mark));
    },
    deleteStudent(id) {
        const index = this.students.indexOf(id);
        this.students = this.students.filter(student => student.id !== id);
    }
};
const GroupFiltered = studentsGroup.studentsFilter(10);
const MarksFiltered = studentsGroup.marksFilter(6);
studentsGroup.deleteStudent(3);
console.log(GroupFiltered);
console.log(MarksFiltered);
console.log(studentsGroup.students);
//# sourceMappingURL=4_types.js.map