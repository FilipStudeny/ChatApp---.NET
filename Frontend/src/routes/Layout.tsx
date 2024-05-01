import { Container } from "@mui/material";
import React from "react";
import { Outlet } from "react-router-dom";

import Header from "../components/Header";

export const Layout: React.FunctionComponent = () => (
	<>
		<Header />
		<Container maxWidth='lg' sx={{ mt: 10, p: 0 }}>
			<Outlet />
		</Container>
	</>
);
