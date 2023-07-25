import students from "./IT.json" assert {type: 'json'};

let trElem = document.querySelector('.addSubject');
let subjects = students[0].Subjects;

subjects.forEach(subject => {
    trElem.innerHTML += "<th>" + subject.SubjectName + "</th>";
});


let tableElem = document.querySelector('.myTable');
let tableElemChildIndex = 2;
let trElemChildIndex = 4;
students.forEach(student => {
    let newElem = document.createElement('tr');
    tableElem.appendChild(newElem);
    let no = tableElemChildIndex - 1;
    tableElem.childNodes[tableElemChildIndex].innerHTML += '<td>' + no +'</td>';
    tableElem.childNodes[tableElemChildIndex].innerHTML += '<td>' + student.ID +'</td>';
    tableElem.childNodes[tableElemChildIndex].innerHTML += '<td>' + student.StudentFullName +'</td>';
    let avg = student.AVG.toFixed(1);
    tableElem.childNodes[tableElemChildIndex].innerHTML += '<td>' + avg +'</td>';
    student.Subjects.forEach(subject =>{
        tableElem.childNodes[tableElemChildIndex].innerHTML += '<td>' + subject.FullGrade +'</td>';
    });
    tableElemChildIndex++;
});

console.log(trElem.childElementCount);