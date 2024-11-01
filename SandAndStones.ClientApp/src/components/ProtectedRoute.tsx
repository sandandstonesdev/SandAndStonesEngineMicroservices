import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthProvider";
import { PropsWithChildren } from "react";
import { AuthData } from "../types/AuthData";

type ProtectedRouteProps = PropsWithChildren;

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
    const { isAuthenticated } = useAuth() as AuthData;

    if (!isAuthenticated) {
        return <Navigate to="/login" />;
    }

    return children;
}