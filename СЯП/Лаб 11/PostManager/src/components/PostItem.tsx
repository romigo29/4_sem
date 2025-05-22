import { useState } from 'react'
import { useAppDispatch } from "../types/hooks";
import { editPost, removePost } from "../features/postsSlice";
import { type Post } from "../types/post";

interface Props {
    post: Post;
}

export default function PostItem({ post }: Props) {

    const dispatch = useAppDispatch();
    const [isEditing, setIsEditing] = useState(false);
    const [title, setTitle] = useState(post.title);
    const [body, setBody] = useState(post.body);

    const handleUpdate = () => {
        dispatch(editPost({ ...post, title, body }))
        setIsEditing(false);
    }

    const handleDelete = () => {
        dispatch(removePost(post.id));
    }


    return (
        <div className="post-item fade-in">
            {isEditing ? (
                <div className="post-form">
                    <div className="form-group">
                        <input
                            className="form-control"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            placeholder="Заголовок"
                        />
                    </div>
                    <div className="form-group">
                        <textarea
                            className="form-control"
                            value={body}
                            onChange={(e) => setBody(e.target.value)}
                            placeholder="Содержание"
                        />
                    </div>
                    <div className="post-actions">
                        <button className="btn btn-primary" onClick={handleUpdate}>Сохранить</button>
                        <button className="btn btn-secondary" onClick={() => setIsEditing(false)}>Отмена</button>
                    </div>
                </div>
            ) : (
                <div>
                    <h3 className="post-title">{post.title}</h3>
                    <p className="post-body">{post.body}</p>
                    <div className="post-actions">
                        <button className="btn btn-primary" onClick={() => setIsEditing(true)}>Редактировать</button>
                        <button className="btn btn-danger" onClick={handleDelete}>Удалить</button>
                    </div>
                </div>
            )}
        </div>
    );
};