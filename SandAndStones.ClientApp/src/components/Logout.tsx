import { useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom';
import { axiosInstance } from '../hooks/useAxios';
import { isAxiosError } from 'axios';
import { useDispatch } from 'react-redux';
import { setUnauthenticated } from "../redux/store/AuthSlice"

function Logout() {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    useEffect(() => {
        const handleLogout = async () => {
            try {
                const response = await axiosInstance.get(`${import.meta.env.VITE_APP_BASE_URL}/gateway-api/userAuthorization/logout`);
                console.log(response.data);
                dispatch(setUnauthenticated());
                navigate('/', { replace: true });
                console.info("Logout successful.");
            }
            catch (err: unknown) {
                if (isAxiosError(err)) {
                    if (err?.response) {

                        console.error(`Logout error. Details: ${err.response.status} ${err.response.statusText}`)
                    }
                    else {
                        console.error("Logout error: No response");
                    }
                }
            }
        };

        handleLogout()
    });

    return (
        <>
            <Navigate to="/login" replace={true} />
        </>);
}

export default Logout;