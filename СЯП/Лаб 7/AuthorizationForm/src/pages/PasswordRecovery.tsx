import React from "react"
import { useState } from 'react'
import '../css/App.css'
import { Link } from 'react-router-dom'
import { storage } from "../localStorage/localStorage"

function Authorization() {



    const [email, setEmail] = useState('')
    const [emailInvalid, setEmailInvalid] = useState(false)
    const [emailError, setEmailError] = useState('Email не должен быть пустым')
    const [password, setPassword] = useState('');
    const [passwordExists, setPasswordExists] = useState(false)
    const [recoveryError, setRecoveryError] = useState('')


    const blurHandle = (e: React.ChangeEvent<HTMLInputElement>) => {
        switch (e.target.name) {
            case ('email'): {
                setEmailInvalid(true);
                break;
            }
        }
    }

    const handleEmail = (e: React.ChangeEvent<HTMLInputElement>) => {
        const currentEmail = e.target.value;
        setEmail(currentEmail);
        const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        if (!emailRegex.test(String(currentEmail))) {
            setEmailError("Неверно введенный email")
            return;
        }

        if (storage.emailExists(currentEmail)) {
            setEmailError("Email уже существует!");
        }

        setEmailError('')

    }

    const handleSubmit = (e: React.FormEvent<HTMLButtonElement>) => {
        e.preventDefault();

        if (!email) {
            setPasswordExists(false);
            setRecoveryError("Заполните все поля в форме!");
            return;
        }

        if (emailError) {
            setPasswordExists(false);
            setRecoveryError("Правильно заполните форму!");
            return;
        }

        if (!storage.emailExists(email)) {
            setPasswordExists(false);
            setRecoveryError("Пользователя с таким email не существует!");
            return;
        }

        setPassword(storage.getPassword(email))
        setPasswordExists(true);
        setRecoveryError('');
    }



    return (
        <div className="formCard">
            <form >

                <h1>Восстановление пароля</h1>
                <div className="fields">
                    {(emailInvalid && emailError) && <div className="errorMessage">{emailError}</div>}
                    <input type="email" name="email" id="" placeholder="Email" required value={email} onBlur={blurHandle} onChange={e => handleEmail(e)} />
                    <button type="submit" onClick={e => handleSubmit(e)}>Подтвердить</button>
                </div>
                <div className="links">
                    <Link to="/sign-in">Вернуться к авторизации</Link>
                </div>
                {(!passwordExists && recoveryError) && <div className="collapseMessage">{recoveryError}</div>}
                {(passwordExists && !recoveryError) && <div className="successMessage">Ваш пароль: {password}</div>}
            </form>
        </div>
    );
}

export default Authorization
