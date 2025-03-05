type MarkFilterType = 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10;
type DoneType = true | false;
type GroupFilterType = 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12;

type MarkType = {
    subject: string,
    mark: MarkFilterType, 
    done: DoneType,
}

type StudentType = {
    id: number,
    name: string,
    group: GroupFilterType, 
    marks: Array<MarkType>,
}

type GroupType = {
    students: Array<StudentType>,
    mark: MarkFilterType,
    group: GroupFilterType,
    studentsFilter: (group: number) => Array<StudentType>,  //фильтр по группе
    marksFilter: (mark: number) => Array<StudentType>   //фильтр по оценке
    deleteStudent: (id: number) => void, //удалить студент по id из исходного массива

}

const studentsGroup: GroupType = {
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
    studentsFilter(group: number) {
        return this.students.filter(student => student.group == group)
    },

    marksFilter(mark: number) {
        return this.students.filter(student => student.marks.some(m => m.mark == mark))
    },

    deleteStudent(id: number) {
        const index = this.students.indexOf(id)
        this.students = this.students.filter(student => student.id !== id)
    }

}

const GroupFiltered: StudentType[] = studentsGroup.studentsFilter(10);
const MarksFiltered: StudentType[] = studentsGroup.marksFilter(6);
studentsGroup.deleteStudent(3);

console.log(GroupFiltered);
console.log(MarksFiltered);
console.log(studentsGroup.students);