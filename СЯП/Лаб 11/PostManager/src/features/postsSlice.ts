import { createSlice, createAsyncThunk, type PayloadAction } from "@reduxjs/toolkit";
import type { Post } from "../types/post";
import * as postsAPI from "./postAPI"

interface PostsState {
    posts: Post[],
    loading: boolean,
    error: string | null
}

const initialState: PostsState = {
    posts: [],
    loading: false,
    error: null
}

export const getPosts = createAsyncThunk("posts/fetchPosts", postsAPI.fetchPosts);
export const addPost = createAsyncThunk("posts/createPost", postsAPI.createPost);
export const editPost = createAsyncThunk("posts/updatePost", postsAPI.updatePost);
export const removePost = createAsyncThunk("posts/deletePost", postsAPI.deletePost);

const postsSlice = createSlice({
    name: "posts",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(getPosts.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(getPosts.fulfilled, (state, action: PayloadAction<Post[]>) => {
                state.posts = action.payload;
                state.loading = false;
            })
            .addCase(getPosts.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || "Failed to load posts.";
            })
            .addCase(addPost.fulfilled, (state, action: PayloadAction<Post>) => {
                state.posts.unshift(action.payload);
            })
            .addCase(editPost.fulfilled, (state, action: PayloadAction<Post>) => {
                const index = state.posts.findIndex(p => p.id === action.payload.id);
                if (index !== -1) {
                    state.posts[index] = action.payload;
                }
            })
            .addCase(removePost.fulfilled, (state, action) => {
                state.posts = state.posts.filter(p => p.id !== action.meta.arg);
            });
    },
});

export default postsSlice.reducer
