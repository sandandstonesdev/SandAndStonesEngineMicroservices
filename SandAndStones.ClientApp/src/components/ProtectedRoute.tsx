import { Navigate } from "react-router-dom";
import { PropsWithChildren } from "react";
import { useAppSelector } from "../redux/store/Store";

type ProtectedRouteProps = PropsWithChildren;

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
    const isAuthenticated = useAppSelector(state => state.auth.isAuthenticated);
    
    if (!isAuthenticated) {
        return <Navigate to="/login" />;
    }

    return children;
}