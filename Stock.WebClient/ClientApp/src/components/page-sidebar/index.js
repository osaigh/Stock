import React from "react";
import {
  NavLink,
  UnOrderedList,
  ListItem,
  AsideContainer,
} from "./styles/page-sidebar";

export default function PageSideBar({ children, ...restProps }) {
  return <AsideContainer {...restProps}>{children}</AsideContainer>;
}

PageSideBar.NavLink = function PageSideBarNavLink({ children, ...restProps }) {
  return <NavLink {...restProps}>{children}</NavLink>;
};

PageSideBar.UnOrderedList = function PageSideBarUnOrderedList({
  children,
  ...restProps
}) {
  return <UnOrderedList {...restProps}>{children}</UnOrderedList>;
};

PageSideBar.ListItem = function PageSideBarListItem({
  children,
  ...restProps
}) {
  return <ListItem {...restProps}>{children}</ListItem>;
};
