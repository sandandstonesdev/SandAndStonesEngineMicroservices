import { configureStore } from '@reduxjs/toolkit';
import { authReducer } from './AuthSlice';
import { useSelector } from 'react-redux';

export const store = configureStore({
    reducer: {
        auth: authReducer
    },
});

export type AppDispatch = typeof store.dispatch;
export type AuthorizeState = ReturnType<typeof store.getState>;
export const useAppSelector = useSelector.withTypes<AuthorizeState>();