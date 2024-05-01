/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { createTheme, ThemeProvider } from "@mui/material";
import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";

import App from "./App";

import "./index.css";

const themeOptions = createTheme({
	palette: {
		mode: "dark",
		primary: {
			main: "#8985f2",
		},
		secondary: {
			main: "#ff4843",
		},
	},
});

ReactDOM.createRoot(document.getElementById("root")!).render(
	<React.StrictMode>
		<BrowserRouter>
			<ThemeProvider theme={themeOptions}>
				<App />
			</ThemeProvider>
		</BrowserRouter>
	</React.StrictMode>,
);
