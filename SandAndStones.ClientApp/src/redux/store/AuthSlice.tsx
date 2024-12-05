import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { axiosInstance } from '../../hooks/useAxios';
import { isAxiosError } from 'axios';
import { AuthState } from '../../types/AuthState';

const name = 'auth';

export const checkTokenValidity = createAsyncThunk(
    `${name}/checkTokenValidity`,
    async (_, thunkApi ) => {
    try {
        const response = await axiosInstance.get(
            `${import.meta.env.VITE_APP_BASE_URL}/gateway-api/userAuthorization/currenttokenvalid`
        );

        return thunkApi.fulfillWithValue({ isAuthenticated: response.data.isValid });
    }
    catch (err: unknown) {
        if (isAxiosError(err)) {
            if (err.response) {
                console.error(`Error while Current Token Validation Status: ${err.response.status} ${err.response.data}`);
            }
            else {
                console.error(`Error while Current Token Validation: ${err}`);
            }
        }

        return thunkApi.rejectWithValue({ error: `Error while Current Token Validation: ${err}` });
    }
});

const initialState = {
    isAuthenticated: false,
    error: ""
} as AuthState

const authSlice = createSlice({
    name,
    initialState: initialState,
    reducers: {
        setAuthenticated: (state) => {
            state.isAuthenticated = true;
        },
        setUnauthenticated: (state) => {
            state.isAuthenticated = false;
        }

    },
    extraReducers: builder => {
        builder
            .addCase(checkTokenValidity.fulfilled, (state, action) => {
                console.info(action.payload);
                return { ...state, isAuthenticated: true }
            })
            .addCase(checkTokenValidity.rejected, (state, action) => {
                console.error(action.payload);
                return { ...state, isAuthenticated: false }
            })
    }
});

export const { setAuthenticated, setUnauthenticated } = authSlice.actions;
export const authReducer = authSlice.reducer;
export const isAuthenticatedFlag = (state: AuthState) => state.isAuthenticated;
