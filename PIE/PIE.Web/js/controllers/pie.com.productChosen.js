angular.module('pie').controller('ProductChosen', ['$scope','$timeout', function ($scope,$timeout) {
    console.log(">>>>ProductChosen");
   
    var init = function () {
        console.log(">>PlanCreation.init :: $scope.curComponent");
        console.log($scope.curComponent);
        if ($scope.curComponent) {
            var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + $scope.curComponent.name + '&nbsp;({itemId})'.replace('{itemId}', $scope.curComponent.id) + '</div>';
            $scope.selProduct.setContent(dropDownContent);
        }
        else {
            var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px; color: rgba(162, 162, 162, 1)">Select Product</div>';
            $scope.selProduct.setContent(dropDownContent);
        }
    };

    $scope.$on('treeProductInitialized', function (event, arguments) {
        console.log(">>PlanCreation.treeProductInitialized :: event, arguments");
        console.log(event);
        console.log(arguments);

        $timeout(function(){
            init();
        }, 500);
    });


    $scope.$on('productSelect', function (event, arguments) {
        console.log(">>PlanCreation.productSelect :: event, arguments");
        console.log(event);
        console.log(arguments);

        var item = $scope.treeProduct.getItem(arguments.args.element);
        var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + item.label + '&nbsp;({itemId})'.replace('{itemId}', item.id) + '</div>';
        $scope.selProduct.setContent(dropDownContent);

        var path = (function getCidPath(item) {
            var path = item.id;
            var parentItem = $scope.treeProduct.getItem(item.parentElement);
            while (parentItem) {
                path = parentItem.id + '/' + path;
                parentItem = $scope.treeProduct.getItem(parentItem.parentElement);
            }
            return path;
        })(item);

        var data = {
            cid: item.id,
            product: item.label,
            path: path
        };

        $scope.$emit('productSelected', data);
    });

    $scope.$on('selProductOpen', function (event, arguments) {
        console.log(">>PlanCreation.selProductOpen :: event, arguments");
        console.log(event);
        console.log(arguments);

        //todo
    });

    $scope.$on('nodeExpand', function (event, arguments) {
        console.log(">>PlanCreation.nodeExpand :: event, arguments");
        console.log(event);
        console.log(arguments);

        //todo
    });
}]);