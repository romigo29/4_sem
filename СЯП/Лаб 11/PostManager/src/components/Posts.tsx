import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../types/hooks"
import { getPosts } from "../features/postsSlice";
import PostItem from "./PostItem";
import PostForm from "./PostForm";

export default function Posts() {
    const dispatch = useAppDispatch()
    const { posts, loading, error } = useAppSelector((state) => state.posts);

    useEffect(() => {
        dispatch(getPosts());
    }, [dispatch]);

    if (loading) return <p className="loading">Загрузка...</p>;
    if (error) return <p className="error">Ошибка: {error}</p>;

    return (
        <div className="fade-in">
            <PostForm />
            <div className="posts-list">
                {posts.map((post) => (
                    <PostItem key={post.id} post={post} />
                ))}
            </div>
        </div>
    )
}
