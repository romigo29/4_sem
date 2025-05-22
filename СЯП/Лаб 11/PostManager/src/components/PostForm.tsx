import React, { useState } from "react";
import { useAppDispatch } from "../types/hooks";
import { addPost } from "../features/postsSlice";


export default function PostForm() {

    const dispatch = useAppDispatch();
    const [title, setTitle] = useState("");
    const [body, setBody] = useState("");

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        dispatch(addPost({ title, body }));
        setTitle("");
        setBody("");
    }

    return (
        <div className="post-form fade-in">
            <h2>Создать новый пост</h2>
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="title">Заголовок</label>
                    <input
                        id="title"
                        className="form-control"
                        placeholder="Введите заголовок"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="body">Содержание</label>
                    <textarea
                        id="body"
                        className="form-control"
                        placeholder="Введите текст поста"
                        value={body}
                        onChange={(e) => setBody(e.target.value)}
                        required
                    />
                </div>
                <button className="btn btn-primary" type="submit">Создать пост</button>
            </form>
        </div>
    );
}
